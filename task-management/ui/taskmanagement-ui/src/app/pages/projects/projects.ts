import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { ProjectService, Project } from '../../services/project.service';
import { TaskService, TaskItem, TaskStatusLabels } from '../../services/task.service';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './projects.html',
  styleUrl: './projects.css'
})
export class ProjectsComponent implements OnInit {
  projects = signal<Project[]>([]);
  tasks = signal<TaskItem[]>([]);
  selectedProject = signal<Project | null>(null);
  loading = signal(true);
  showCreateModal = signal(false);
  showTaskModal = signal(false);
  showMemberModal = signal(false);

  newProject = { name: '', description: '' };
  newTask = { title: '', description: '', assigneeUserId: '', dueDate: '' };
  newMember = { userId: '' };

  taskStatusLabels = TaskStatusLabels;

  constructor(
    private authService: AuthService,
    private projectService: ProjectService,
    private taskService: TaskService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    this.projectService.getProjects().subscribe({
      next: (projects) => {
        this.projects.set(projects);
        this.loading.set(false);

        this.route.params.subscribe(params => {
          const projectId = params['id'];
          if (projectId && projects.length > 0) {
            const project = projects.find(p => p.id === projectId);
            if (project) {
              this.selectProject(project);
            }
          }
        });
      },
      error: () => {
        this.loading.set(false);
      }
    });
  }

  selectProject(project: Project): void {
    this.selectedProject.set(project);
    this.loadTasks(project.id);
  }

  loadTasks(projectId: string): void {
    this.taskService.getTasksByProject(projectId).subscribe({
      next: (tasks) => {
        this.tasks.set(tasks);
      }
    });
  }

  createProject(): void {
    if (!this.newProject.name) return;

    this.projectService.createProject(this.newProject).subscribe({
      next: (project) => {
        this.projects.update(projects => [...projects, project]);
        this.showCreateModal.set(false);
        this.newProject = { name: '', description: '' };
      }
    });
  }

  createTask(): void {
    if (!this.newTask.title || !this.selectedProject()) return;

    this.taskService.createTask({
      projectId: this.selectedProject()!.id,
      ...this.newTask
    }).subscribe({
      next: (task) => {
        this.tasks.update(tasks => [...tasks, task]);
        this.showTaskModal.set(false);
        this.newTask = { title: '', description: '', assigneeUserId: '', dueDate: '' };
      }
    });
  }

  addMember(): void {
    if (!this.newMember.userId || !this.selectedProject()) return;

    this.projectService.addMember(this.selectedProject()!.id, this.newMember.userId).subscribe({
      next: () => {
        this.showMemberModal.set(false);
        this.newMember = { userId: '' };
      }
    });
  }

  updateTaskStatus(taskId: string, status: number): void {
    this.taskService.updateTaskStatus(taskId, status as 0 | 1 | 2).subscribe({
      next: () => {
        this.tasks.update(tasks =>
          tasks.map(t => t.id === taskId ? { ...t, status: status as 0 | 1 | 2 } : t)
        );
      }
    });
  }

  logout(): void {
    this.authService.logout();
  }

  isAdmin(): boolean {
    return this.authService.userRole() === 'Admin';
  }

  isOverdue(dueDate: string | null, status: number): boolean {
    if (!dueDate || status === 2) return false;
    const today = new Date().toISOString().split('T')[0];
    return dueDate < today;
  }

  canUpdateTask(task: TaskItem): boolean {
    if (this.isAdmin()) return true;
    const currentUserId = this.authService.userId();
    return task.assigneeUserId === currentUserId;
  }
}

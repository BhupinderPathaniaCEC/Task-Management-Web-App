import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { ProjectService, Project } from '../../services/project.service';
import { TaskService, TaskItem } from '../../services/task.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent implements OnInit {
  projects = signal<Project[]>([]);
  tasks = signal<TaskItem[]>([]);
  loading = signal(true);

  constructor(
    private authService: AuthService,
    private projectService: ProjectService,
    private taskService: TaskService,
    private router: Router
  ) {}

  get userRole() {
    return this.authService.userRole;
  }

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.projectService.getProjects().subscribe({
      next: (projects) => {
        this.projects.set(projects);
        this.loadTasksForProjects(projects);
      },
      error: () => {
        this.loading.set(false);
      }
    });
  }

  loadTasksForProjects(projects: Project[]): void {
    if (projects.length === 0) {
      this.loading.set(false);
      return;
    }

    const firstProject = projects[0];
    this.taskService.getTasksByProject(firstProject.id).subscribe({
      next: (tasks) => {
        this.tasks.set(tasks);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      }
    });
  }

  logout(): void {
    this.authService.logout();
  }

  getOverdueTasks(): number {
    const today = new Date().toISOString().split('T')[0];
    return this.tasks().filter(t => t.dueDate && t.dueDate < today && t.status !== 2).length;
  }

  getCompletedTasks(): number {
    return this.tasks().filter(t => t.status === 2).length;
  }

  getPendingTasks(): number {
    return this.tasks().filter(t => t.status !== 2).length;
  }

  isOverdue(dueDate: string | null, status: number): boolean {
    if (!dueDate || status === 2) return false;
    const today = new Date().toISOString().split('T')[0];
    return dueDate < today;
  }
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export type TaskStatus = 0 | 1 | 2;

export const TaskStatusLabels: Record<TaskStatus, string> = {
  0: 'Todo',
  1: 'In Progress',
  2: 'Done'
};

export interface TaskItem {
  id: string;
  projectId: string;
  title: string;
  description: string | null;
  status: TaskStatus;
  dueDate: string | null;
  assigneeUserId: string | null;
  createdAt: string;
}

export interface CreateTaskRequest {
  projectId: string;
  title: string;
  description?: string;
  assigneeUserId?: string;
  dueDate?: string;
}

export interface UpdateTaskStatusRequest {
  status: TaskStatus;
}

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getTasksByProject(projectId: string): Observable<TaskItem[]> {
    return this.http.get<TaskItem[]>(`${this.apiUrl}/tasks/project/${projectId}`);
  }

  createTask(request: CreateTaskRequest): Observable<TaskItem> {
    return this.http.post<TaskItem>(`${this.apiUrl}/tasks`, request);
  }

  updateTaskStatus(taskId: string, status: TaskStatus): Observable<void> {
    const request: UpdateTaskStatusRequest = { status };
    return this.http.patch<void>(`${this.apiUrl}/tasks/${taskId}/status`, request);
  }
}

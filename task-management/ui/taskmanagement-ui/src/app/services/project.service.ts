import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Project {
  id: string;
  name: string;
  description: string | null;
  createdAt: string;
}

export interface CreateProjectRequest {
  name: string;
  description?: string;
}

export interface AddMemberRequest {
  userId: string;
}

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getProjects(): Observable<Project[]> {
    return this.http.get<Project[]>(`${this.apiUrl}/projects`);
  }

  createProject(request: CreateProjectRequest): Observable<Project> {
    return this.http.post<Project>(`${this.apiUrl}/projects`, request);
  }

  addMember(projectId: string, userId: string): Observable<void> {
    const request: AddMemberRequest = { userId };
    return this.http.post<void>(`${this.apiUrl}/projects/${projectId}/members`, request);
  }
}

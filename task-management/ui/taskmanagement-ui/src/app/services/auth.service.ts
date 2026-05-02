import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';

interface AuthResponse {
  accessToken: string;
}

interface LoginRequest {
  email: string;
  password: string;
}

interface RegisterRequest {
  email: string;
  password: string;
  role: 'Admin' | 'Member';
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = 'https://localhost:5001/api';
  
  readonly isAuthenticated = signal(!!localStorage.getItem('access_token'));
  readonly userRole = signal<string | null>(localStorage.getItem('user_role'));

  constructor(private http: HttpClient, private router: Router) {}

  login(credentials: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/login`, credentials).pipe(
      tap(response => {
        localStorage.setItem('access_token', response.accessToken);
        this.isAuthenticated.set(true);
        this.decodeAndStoreRole(response.accessToken);
      })
    );
  }

  register(data: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/register`, data).pipe(
      tap(response => {
        localStorage.setItem('access_token', response.accessToken);
        this.isAuthenticated.set(true);
        this.decodeAndStoreRole(response.accessToken);
      })
    );
  }

  logout(): void {
    localStorage.removeItem('access_token');
    localStorage.removeItem('user_role');
    this.isAuthenticated.set(false);
    this.userRole.set(null);
    this.router.navigate(['/login']);
  }

  private decodeAndStoreRole(token: string): void {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const role = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || 
                   payload.role || 
                   null;
      if (role) {
        localStorage.setItem('user_role', role);
        this.userRole.set(role);
      }
    } catch {
      console.error('Failed to decode token');
    }
  }
}

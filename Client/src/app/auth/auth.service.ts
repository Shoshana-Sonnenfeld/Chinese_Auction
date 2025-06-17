import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private token: string | null = null;
  private user: any = null;

  constructor() {}

  setToken(token: string): void {
    this.token = token;
    localStorage.setItem('auth_token', token);
  }

  getToken(): string | null {
    if (!this.token) {
      this.token = localStorage.getItem('auth_token');
    }
    return this.token;
  }

  setUser(user: any): void {
    this.user = user;
    localStorage.setItem('auth_user', JSON.stringify(user));
  }

  getUser(): any {
    if (!this.user) {
      const userStr = localStorage.getItem('auth_user');
      this.user = userStr ? JSON.parse(userStr) : null;
    }
    return this.user;
  }

  clearAuth(): void {
    this.token = null;
    this.user = null;
    localStorage.removeItem('auth_token');
    localStorage.removeItem('auth_user');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}

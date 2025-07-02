import { Component } from '@angular/core';
import { Auth } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';
import { AvatarModule } from 'primeng/avatar';
import { OverlayPanelModule } from 'primeng/overlaypanel';

@Component({
  standalone: true,
  selector: 'app-home',
  imports: [CommonModule, CardModule, AvatarModule, OverlayPanelModule],
 templateUrl: './home.html',
})
export class HomeComponent {
  userName: string = '';
  initials: string = '';
  showLogout: boolean = false;
  constructor(private auth: Auth) {
    this.userName = this.auth.getFullName() || '';
    if (this.userName) {
      const parts = this.userName.trim().split(' ');
      this.initials = parts.map(p => p.charAt(0)).join('').toUpperCase();
    }
  }

  logout() {
    localStorage.removeItem('token');
    window.location.href = '/login';
  }
}

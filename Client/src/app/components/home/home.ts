import { Component } from '@angular/core';
import { Auth } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';

@Component({
  standalone: true,
  selector: 'app-home',
  imports: [CommonModule, CardModule],
 templateUrl: './home.html',
})
export class HomeComponent {
  userName: string = '';
  constructor(private auth: Auth) {
    this.userName = this.auth.getFullName() || '';
  }
}

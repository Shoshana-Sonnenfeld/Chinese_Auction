import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { Router, NavigationEnd } from '@angular/router';
import { Auth } from '../../services/auth.service';
import { AvatarModule } from 'primeng/avatar';
import { PopoverModule } from 'primeng/popover';
import { MenubarModule } from 'primeng/menubar';
import { ButtonModule } from 'primeng/button';
import { RouterModule } from '@angular/router';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [
    AvatarModule,
    PopoverModule,
    MenubarModule,
    ButtonModule,
    RouterModule
  ],
  templateUrl: './layout.html',
  styleUrl: './layout.scss'
})
export class Layout implements OnInit {

  items: MenuItem[] = [];
  initials: string = '';
  fullName: string = '';
  currentUrl: string = '';

  constructor(private auth: Auth, private router: Router) {}

  ngOnInit() {
    this.updateMenuItems(this.router.url);

    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: any) => {
        this.currentUrl = event.urlAfterRedirects;
        this.updateMenuItems(this.currentUrl);
      });

    this.fullName = this.auth.getFullName() || '';
    if (this.fullName) {
      const parts = this.fullName.trim().split(' ');
      this.initials = parts.map(p => p.charAt(0)).join('').toUpperCase();
    }
  }

  updateMenuItems(currentUrl: string) {
    this.items = [
      { label: 'Home', icon: 'pi pi-home', routerLink: '/home', styleClass: currentUrl.includes('/home') ? 'active-tab' : '' },
      { label: 'Gifts', icon: 'pi pi-gift', routerLink: '/gifts', styleClass: currentUrl.includes('/gifts') && !currentUrl.includes('giftsManager') ? 'active-tab' : '' },
      { label: 'Gifts_Manager', icon: 'pi pi-gift', routerLink: '/giftsManager', styleClass: currentUrl.includes('/giftsManager') ? 'active-tab' : '' },
      { label: 'Raffle', icon: 'pi pi-star', routerLink: '/raffle', styleClass: currentUrl.includes('/raffle') ? 'active-tab' : '' },
      { label: 'Summary', icon: 'pi pi-chart-bar', routerLink: '/summary', styleClass: currentUrl.includes('/summary') ? 'active-tab' : '' },
      { label: 'Donors', icon: 'pi pi-users', routerLink: '/donors', styleClass: currentUrl.includes('/donors') ? 'active-tab' : '' },
      { label: 'Categories', icon: 'pi pi-list', routerLink: '/categories', styleClass: currentUrl.includes('/categories') ? 'active-tab' : '' },
      { label: 'ListGiftsToUser', icon: 'pi pi-list', routerLink: '/giftslist', styleClass: currentUrl.includes('/giftslist') ? 'active-tab' : '' },
      { label: 'Payment', icon: 'pi pi-list', routerLink: '/payment-card', styleClass: currentUrl.includes('/payment-card') ? 'active-tab' : '' }
    ];
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}

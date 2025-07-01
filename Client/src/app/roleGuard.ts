// auth-role.guard.ts
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { Auth } from './services/auth.service';

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {
  constructor(private auth: Auth, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean | UrlTree {
    const expectedRoles: string[] = route.data['roles'] || [];
    const userRole = this.auth.getUserRole();

    if (expectedRoles.includes(userRole)) {
      return true;
    }

    return this.router.parseUrl('/home'); // או '/unauthorized'
  }
}

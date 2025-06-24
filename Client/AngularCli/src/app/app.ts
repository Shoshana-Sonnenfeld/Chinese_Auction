import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PrimengDemo } from './primeng-demo/primeng-demo';
import { MenuItem } from 'primeng/api';
import { MenubarModule } from 'primeng/menubar';
import { CommonModule } from '@angular/common'; 
import { RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { PanelModule } from 'primeng/panel';



@Component({
  selector: 'app-root',
  imports: [RouterOutlet,
        MenubarModule,
    CommonModule,
    RouterModule,
    CardModule,
    PanelModule
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected title = 'base-app';


   items: MenuItem[] = [];

  ngOnInit() {
    this.items = [
      { label: 'Home', icon: 'pi pi-home', routerLink: '/home' },
      { label: 'Gifts', icon: 'pi pi-home', routerLink: '/gifts' }
    ];
  }
}

import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login';
import { SignUpComponent } from './components/sign-up-component/sign-up-component';
import { CategoryComponent } from './components/category/category';
import { DonorComponent } from './components/donor/donor';
import { GiftComponent } from './components/gifts/gifts';
import { HomeComponent } from './components/home/home';
import { GiftAddComponent } from './components/gifts/gift-add/gift-add';
import { Layout } from './components/layout/layout';

export const routes: Routes = [

  // דף ברירת מחדל - הפניה ל־login
  { path: '', redirectTo: 'login', pathMatch: 'full' },


  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignUpComponent },

  // דפים עם תפריט + פוטר
  {
    path: '',
    component: Layout,
    children: [
      { path: 'home', component: HomeComponent },
      { path: 'categories', component: CategoryComponent },
      { path: 'donors', component: DonorComponent },
      { path: 'gifts', component: GiftComponent },
      { path: 'gifts/add', component: GiftAddComponent },
    ]
  },

  // כתובת לא קיימת → דף הבית
  { path: '**', redirectTo: 'home' }
];

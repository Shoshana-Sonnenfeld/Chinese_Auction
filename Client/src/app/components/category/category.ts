import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../../services/category.service';
import { Category } from '../../models/category.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';  
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';

@Component({
  selector: 'app-category',
  templateUrl: './category.html',
  styleUrls: ['./category.scss'],
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, ButtonModule, InputTextModule, DropdownModule]
})
export class CategoryComponent implements OnInit {
  categories: Category[] = [];
  newCategoryName: string = '';
  loading: boolean = false;  // הוספת משתנה loading

  // אם תרצי להשתמש במיפוי ל-Dropdown (במידה ויש שימוש)
  get categoryOptions() {
    return this.categories.map(c => ({ label: c.name, value: c.id }));
  }

  constructor(private categoryservice: CategoryService) { }

  ngOnInit(): void {
    this.getAllCategories();
  }

  getAllCategories() {
    this.loading = true;
    this.categoryservice.getAllCategories().subscribe({
      next: data => {
        this.categories = Array.isArray(data) ? data : [];
        this.loading = false;
      },
      error: () => {
        this.categories = [];
        this.loading = false;
      }
    });
  }

  addCategory() {
    const newCategory = this.newCategoryName.trim();
    if (!newCategory) {
      // ניהול שגיאה או הודעה למשתמש
      return;
    }

    this.loading = true;
    this.categoryservice.addCategory(newCategory).subscribe({
      next: category => {
        this.categories.push(category);
        this.newCategoryName = '';
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  // שאר הפונקציות כמו saveCategory, deleteCategory נשארות כפי שהן
}

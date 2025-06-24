import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';     
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';
import { RouterModule, Router } from '@angular/router';
import { GiftService } from '../../../services/gift.service';
import { CategoryService } from '../../../services/category.service';
import { DonorService } from '../../../services/donor.service';
import { Category } from '../../../models/category.model';
import { Donor } from '../../../models/donor.model';

@Component({
  selector: 'app-gift-add',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    InputTextModule,
    DropdownModule,
    ButtonModule,
    RouterModule
  ],
  templateUrl: './gift-add.html',
  styleUrls: ['./gift-add.scss']
})
export class GiftAddComponent implements OnInit {
  giftForm: FormGroup;
  categories: Category[] = [];
  donors: Donor[] = [];

  get categoryOptions() {
    return this.categories.map(c => ({ label: c.name, value: c.id }));
  }

  get donorOptions() {
    return this.donors.map(d => ({ label: d.name, value: d.id }));
  }

  message: string = '';
  isError: boolean = false;
  isSubmitting: boolean = false;

  constructor(
    private fb: FormBuilder,
    private giftService: GiftService,
    private router: Router,
    private categoryService: CategoryService,
    private donorService: DonorService
  ) {
    this.giftForm = this.fb.group({
      giftName: ['', Validators.required],
      categoryId: [null, Validators.required],
      donorId: [null, Validators.required],
      price: [0, [Validators.required, Validators.min(0)]],
      details: [''],
      imageUrl: ['']
    });
  }

  ngOnInit() {
    this.categoryService.getAllCategories().subscribe(res => this.categories = Array.isArray(res) ? res : []);
    this.donorService.getAllDonors().subscribe(res => this.donors = Array.isArray(res) ? res : []);
  }

  onSubmit() {
    if (this.giftForm.valid) {
      this.isSubmitting = true;
      this.message = '';

      this.giftService.addGift(this.giftForm.value).subscribe({
        next: () => {
          this.isError = false;
          this.message = 'Gift saved successfully.';
          this.isSubmitting = false;
          this.router.navigate(['/gifts']);
        },
        error: (err) => {
          this.isError = true;
          this.message = 'Failed to save gift. Please try again.';
          this.isSubmitting = false;
          console.error(err);
        }
      });
    }
  }

  isInvalid(controlName: string): boolean {
    const control = this.giftForm.get(controlName);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }
}

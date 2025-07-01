import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { GiftService } from '../../../services/gift.service';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-gift-update',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './gift-update.html',
  styleUrl: './gift-update.scss'
})
export class GiftUpdate {
  giftForm: FormGroup;
  selectedFile: File | null = null;
  previewUrl: string | ArrayBuffer | null = null;
  isSubmitting = false;
  message = '';
  isError = false;
  giftId: number;

  constructor(private fb: FormBuilder, private giftService: GiftService, private router: Router, private route: ActivatedRoute) {
    this.giftId = +this.route.snapshot.paramMap.get('id')!;
    this.giftForm = this.fb.group({
      giftName: ['', Validators.required],
      categoryId: [null, Validators.required],
      donorId: [null, Validators.required],
      price: [0, [Validators.required, Validators.min(0)]],
      details: [''],
      imageUrl: ['']
    });
    this.loadGift();
  }

  loadGift() {
    this.giftService.getGiftById(this.giftId).subscribe(gift => {
      let imageUrl = gift.imageUrl;
      if (imageUrl && imageUrl.startsWith('/images')) {
        imageUrl = 'https://localhost:5001' + imageUrl;
      }
      this.giftForm.patchValue({ ...gift, imageUrl });
    });
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      const reader = new FileReader();
      reader.onload = e => this.previewUrl = reader.result;
      reader.readAsDataURL(file);
    }
  }

  async onSubmit() {
    this.isSubmitting = true;
    this.isError = false;
    this.message = '';
    try {
      let imageUrl = this.giftForm.value.imageUrl;
      if (this.selectedFile) {
        const formData = new FormData();
        formData.append('file', this.selectedFile);
        const uploadRes: any = await this.giftService.uploadImage(formData).toPromise();
        imageUrl = uploadRes.imageUrl;
        this.giftForm.patchValue({ imageUrl });
      }
      await this.giftService.updateGift(this.giftId, this.giftForm.value).toPromise();
      this.router.navigate(['/gifts']);
    } catch (err: any) {
      this.isError = true;
      this.message = err.error?.message || 'Error updating gift';
    } finally {
      this.isSubmitting = false;
    }
  }
}

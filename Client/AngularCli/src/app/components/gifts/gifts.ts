import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GiftService } from '../../services/gift.service';
import { Gift } from '../../models/gift.model';
import { RouterModule, Router } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';
import { DialogModule } from 'primeng/dialog';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-gifts',
  standalone: true,
  imports: [CommonModule, TableModule, ButtonModule, RouterModule, ConfirmDialogModule,DialogModule,FormsModule  ],
  providers: [ConfirmationService],
  templateUrl: './gifts.html'
})
export class GiftComponent implements OnInit {
  gifts: Gift[] = [];
  editDialogVisible: boolean = false;
  selectedGift: any = null;

  constructor(
    private giftService: GiftService,
    private router: Router,
    private confirmationService: ConfirmationService
  ) { }

  ngOnInit() {
    this.giftService.getAllGifts().subscribe((data: any) => {
      this.gifts = data.$values ?? [];
    });
  }

  onAddGift() {
    this.router.navigate(['/gifts/add']);
  }

  onEditGift(gift: any) {
    this.selectedGift = { ...gift }; // העתקה כדי לא לערוך ישירות בטבלה
    this.editDialogVisible = true;
  }

  onSaveEdit() {
  // כאן אפשר לעדכן את השרת או את הרשימה המקומית
  const index = this.gifts.findIndex(g => g.id === this.selectedGift.id);
  if (index !== -1) {
    this.gifts[index] = { ...this.selectedGift };
  }
  this.editDialogVisible = false;
}

  onDeleteGift(id: number) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this gift?',
      header: 'Confirm Deletion',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.giftService.deleteGift(id).subscribe({
          next: () => {
            // סינון מוצרים תקינים בלבד
            this.gifts = this.gifts.filter(g => g && g.id !== id);
          },
          error: err => {
            console.error('Failed to delete gift', err);
          }
        });
      }
    });
  }

  

}

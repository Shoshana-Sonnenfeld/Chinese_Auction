import { Component, OnInit } from '@angular/core';
import { GiftService } from '../../services/gift.service';
import { Gift } from '../../models/gift.model';
import { MessageService } from 'primeng/api';

import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ToastModule } from 'primeng/toast';

@Component({
  selector: 'app-raffle-gift',
  standalone: true,
  imports: [CommonModule, ButtonModule, CardModule, ToastModule],
  templateUrl: './raffle-gift.html',
  styleUrls: ['./raffle-gift.scss'],
  providers: [MessageService] // חשוב להוסיף כאן
})
export class RaffleGiftComponent implements OnInit {
  gifts: Gift[] = [];
  raffleResults: { [giftId: number]: string } = {};
  loading = false;

  constructor(
    private giftService: GiftService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.loading = true;
    this.giftService.getAllGifts().subscribe({
      next: (gifts) => {
        this.gifts = gifts;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  raffle(giftId: number) {
    if (this.raffleResults[giftId]) {
      return;
    }

    this.giftService.raffleGift(giftId).subscribe({
      next: (result) => {
        this.raffleResults[giftId] = result;
        this.messageService.add({
          severity: 'success',
          summary: 'Raffle Completed',
          detail: result
        });
      },
      error: (error) => {
        const errorMsg = error?.error || 'Unknown error during raffle';
        this.raffleResults[giftId] = errorMsg;
        this.messageService.add({
          severity: 'error',
          summary: 'Raffle Failed',
          detail: errorMsg
        });
      }
    });
  }
}

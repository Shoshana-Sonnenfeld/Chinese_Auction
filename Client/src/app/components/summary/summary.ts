import { Component } from '@angular/core';
import { GiftService } from '../../services/gift.service';
import { Gift } from '../../models/gift.model';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
// @ts-ignore
import { jsPDF } from 'jspdf';
// @ts-ignore
import autoTable from 'jspdf-autotable';
import { SafeUrlPipe } from './safe-url.pipe';

@Component({
  selector: 'app-summary',
  standalone: true,
  imports: [CommonModule, ButtonModule, SafeUrlPipe],
  templateUrl: './summary.html',
  styleUrls: ['./summary.scss']
})
export class SummaryComponent {
  totalRevenue: number | null = null;
  pdfUrl: string | null = null;
  pdfBlob: Blob | null = null;

  constructor(private giftService: GiftService) {}

  closePdf(): void {
    if (this.pdfUrl) {
      URL.revokeObjectURL(this.pdfUrl);
    }
    this.pdfUrl = null;
    this.pdfBlob = null;
  }

  downloadWinnersReport() {
    this.giftService.getAllGifts().subscribe(gifts => {
      const doc = new jsPDF();
      autoTable(doc, {
        head: [['Gift Name', 'Winner Name']],
        body: gifts.map(gift => [gift.giftName, gift.winner ? gift.winner.fullName : 'No Winner'])
      });
      const pdfBlob = doc.output('blob');
      this.pdfBlob = pdfBlob;
      if (this.pdfUrl) {
        URL.revokeObjectURL(this.pdfUrl);
      }
      this.pdfUrl = URL.createObjectURL(pdfBlob);
      // הורדה אוטומטית (אם רוצים)
      // doc.save('winners-report.pdf');
    });
  }

  downloadPdfFile() {
    if (this.pdfBlob) {
      const a = document.createElement('a');
      a.href = this.pdfUrl!;
      a.download = 'winners-report.pdf';
      a.click();
    }
  }

  showTotalRevenue() {
    this.giftService.getAllGifts().subscribe(gifts => {
      let total = 0;
      gifts.forEach(gift => {
        if (gift.tickets && Array.isArray(gift.tickets)) {
          total += gift.tickets.length * (gift.price || 0);
        }
      });
      this.totalRevenue = total;
    });
  }
}

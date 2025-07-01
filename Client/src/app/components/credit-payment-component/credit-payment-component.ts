import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { InputTextModule } from 'primeng/inputtext';
import { InputMaskModule } from 'primeng/inputmask';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { TicketService } from '../../services/ticket.service';
import { Ticket } from '../../models/ticket.model';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-credit-payment',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    InputTextModule,
    InputMaskModule,
    ButtonModule,
    CardModule,
    ProgressSpinnerModule
  ],
  templateUrl: './credit-payment-component.html',
  styleUrls: ['./credit-payment-component.scss']
})
export class CreditPaymentComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private ticketService = inject(TicketService);
  private fb = inject(FormBuilder);

  paymentForm!: FormGroup;
  tickets: Ticket[] = [];
  totalPrice = 0;
  returnUrl = '/';
  isProcessing = false;
  successMessage = '';
  focusedField: string | null = null;


ngOnInit() {
  this.paymentForm = this.fb.group({
    cardNumber: ['', [Validators.required]],
    cardHolder: ['', [Validators.required]],
    expiry: ['', [Validators.required]],
    cvv: ['', [Validators.required]]
  });

  const query = this.route.snapshot.queryParamMap;

  const ticketId = query.get('ticketId');
  const ticketIds = query.get('ticketIds');
  this.returnUrl = query.get('returnUrl') ?? '/purchase';

  if (ticketId) {
    // תשלום עבור כרטיס בודד
    this.ticketService.getById(+ticketId).subscribe(ticket => {
      this.tickets = [ticket];
      this.totalPrice = ticket.gift.price;
    });
  } else if (ticketIds) {
    // תשלום עבור מספר כרטיסים
    const ids = ticketIds.split(',').filter(id => id).map(id => +id);
    const requests = ids.map(id => this.ticketService.getById(id));

    forkJoin(requests).subscribe(results => {
      this.tickets = results.filter((t): t is Ticket => !!t);
      this.totalPrice = this.tickets.reduce((sum, t) => sum + t.gift.price, 0);
    });
  }
}


  submit() {
    if (this.paymentForm.invalid || this.isProcessing) {
      this.paymentForm.markAllAsTouched();
      return;
    }

    this.isProcessing = true;

    Promise.all(
      this.tickets.map(ticket =>
        this.ticketService.pay(ticket.id).toPromise()
      )
    ).then(() => {
      this.successMessage = 'Payment completed successfully!';
      this.paymentForm.reset();

      setTimeout(() => {
        this.router.navigateByUrl(this.returnUrl);
      }, 2000);
    }).catch(() => {
      alert('Payment failed. Please try again.');
    }).finally(() => {
      this.isProcessing = false;
    });
  }

  onFocus(field: string) {
    this.focusedField = field;
  }

  onBlur() {
    this.focusedField = null;
  }
}

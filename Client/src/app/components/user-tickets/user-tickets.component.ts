import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TicketService } from '../../services/ticket.service';
import { Ticket } from '../../models/ticket.model';
import { CardModule } from 'primeng/card';
import { TagModule } from 'primeng/tag';
import { CategoryService } from '../../services/category.service';
import { Category } from '../../models/category.model';

@Component({
  selector: 'app-user-tickets',
  standalone: true,
  imports: [CommonModule, CardModule, TagModule],
  templateUrl: './user-tickets.component.html',
  styleUrls: ['./user-tickets.component.scss']
})
export class UserTicketsComponent implements OnInit {
  private ticketService = inject(TicketService);
  private categoryService = inject(CategoryService);
  tickets: Ticket[] = [];
  categories: Category[] = [];
  isLoading = true;

  ngOnInit() {
    this.categoryService.getAllCategories().subscribe(categories => {
      this.categories = categories;
      this.loadTickets();
    });
  }

  loadTickets() {
    this.ticketService.getPaid().subscribe({
      next: (data) => {
        // מיפוי categoryName לכל מתנה בכרטיס
        this.tickets = data.map(ticket => {
          if (ticket.gift && !ticket.gift.categoryName) {
            const cat = this.categories.find(c => c.id === ticket.gift.categoryId);
            return {
              ...ticket,
              gift: {
                ...ticket.gift,
                categoryName: cat ? cat.name : ticket.gift.categoryId?.toString()
              }
            };
          }
          return ticket;
        });
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load tickets:', err);
        this.isLoading = false;
      }
    });
  }

  public getStatusLabel(): string {
    return 'Paid';
  }

  public getStatusSeverity(): string {
    return 'info';
  }
}

import { Gift } from './gift.model';
import { TicketStatus } from './ticket-status.model';
import { User } from './user.model';

export interface Ticket {
  id: number;
  userId: number;
  giftId: number;
  user: User;
  gift: Gift;
  orderDate: string; // מגיע מהשרת כ-string
  status: TicketStatus; // מגיע מהשרת כ-0/1/2
  // payDate?: string; // לא קיים בשרת
}

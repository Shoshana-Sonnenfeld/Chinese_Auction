import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PendingTicketComponent } from './pending-ticket.component';

describe('PendingTicketComponent', () => {
  let component: PendingTicketComponent;
  let fixture: ComponentFixture<PendingTicketComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PendingTicketComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PendingTicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

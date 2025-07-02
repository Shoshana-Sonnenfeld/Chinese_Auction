import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Gift } from '../models/gift.model';
import { Auth } from './auth.service';
import { Donor } from '../models/donor.model';

@Injectable({
  providedIn: 'root'
})
export class GiftService {

  baseUrl: string = 'https://localhost:5001/api/Gifts';

  constructor(private http: HttpClient, private auth: Auth) { }

  private getAuthHeaders(): HttpHeaders {
    const token = this.auth.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  getAllGifts(): Observable<Gift[]> {
    return this.http.get<Gift[]>(this.baseUrl);
  }

  getGiftById(id: number): Observable<Gift> {
    return this.http.get<Gift>(`${this.baseUrl}/${id}`);
  }

  addGift(gift: Partial<Gift>): Observable<Gift> {
    const newGift = {
      ...gift,
      categoryId:
        typeof (gift.categoryId as any) === 'object' && gift.categoryId !== null
          ? (gift.categoryId as any).value
          : gift.categoryId ?? 0,
      donorId:
        typeof (gift.donorId as any) === 'object' && gift.donorId !== null
          ? (gift.donorId as any).value
          : gift.donorId ?? 0,
    };

    return this.http.post<Gift>(this.baseUrl, newGift, {
      headers: this.getAuthHeaders()
    });
  }

  updateGift(id: number, gift: Partial<Gift>): Observable<any> {
    return this.http.put(`${this.baseUrl}/${id}`, gift, {
      headers: this.getAuthHeaders()
    });
  }

  deleteGift(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`, {
      headers: this.getAuthHeaders()
    });
  }

  searchGifts(giftName?: string, donorName?: string, buyerCount?: number): Observable<Gift[]> {
    let params = new HttpParams();
    if (giftName) {
      params = params.set('giftName', giftName);
    }
    if (donorName) {
      params = params.set('donorName', donorName);
    }
    if (buyerCount !== undefined && buyerCount !== null) {
      params = params.set('buyerCount', buyerCount.toString());
    }

    return this.http.get<Gift[]>(`${this.baseUrl}/search`, { params });
  }

  getDonorByGiftId(giftId: number): Observable<Donor> {
    return this.http.get<Donor>(`${this.baseUrl}/donor/${giftId}`, {
      headers: this.getAuthHeaders()
    });
  }

  sortByPrice(): Observable<Gift[]> {
    return this.http.get<Gift[]>(`${this.baseUrl}/sort/price`);
  }

  sortByCategory(): Observable<Gift[]> {
    return this.http.get<Gift[]>(`${this.baseUrl}/sort/category`);
  }

  raffleGift(giftId: number): Observable<string> {
    return this.http.put(`${this.baseUrl}/raffle/${giftId}`, null, {
      headers: this.getAuthHeaders(),
      responseType: 'text'
    });
  }
}

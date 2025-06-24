import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Gift } from '../models/gift.model';
import { Auth } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class GiftService {

  baseUrl: string = 'https://localhost:5001/api/Gifts';

  constructor(private http: HttpClient, private auth: Auth) {}

  getAllGifts(): Observable<Gift[]> {
    return this.http.get<Gift[]>(this.baseUrl);
  }

  getGiftById(id: number): Observable<Gift> {
    return this.http.get<Gift>(`${this.baseUrl}/${id}`);
  }

  addGift(gift: Partial<Gift>): Observable<Gift> {
    const token = this.auth.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.post<Gift>(this.baseUrl, gift, { headers });
  }

  updateGift(id: number, gift: Partial<Gift>): Observable<any> {
    const token = this.auth.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.put(`${this.baseUrl}/${id}`, gift, { headers });
  }

  deleteGift(id: number): Observable<any> {
    const token = this.auth.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.delete(`${this.baseUrl}/${id}`, { headers });
  }
}

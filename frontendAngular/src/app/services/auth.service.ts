import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = 'https://localhost:7096/api/auth';

  constructor(private http: HttpClient) {}

  register(userData: any): Observable<{ message: string; userId: string }> {
    return this.http.post<{ message: string; userId: string }>(`${this.apiUrl}/register`, userData);
  }
}
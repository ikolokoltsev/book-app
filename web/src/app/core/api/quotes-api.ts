import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { QuoteRequest, QuoteResponse } from './model';

@Injectable({ providedIn: 'root' })
export class QuotesApi {
  private readonly http = inject(HttpClient);
  private readonly url = `${environment.apiUrl}/api/quotes`;

  list() {
    return this.http.get<QuoteResponse[]>(this.url);
  }

  create(quote: QuoteRequest) {
    return this.http.post<QuoteResponse>(this.url, quote);
  }

  update(id: string, quote: QuoteRequest) {
    return this.http.put<QuoteResponse>(`${this.url}/${id}`, quote);
  }

  remove(id: string) {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}

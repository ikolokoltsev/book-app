import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { BookRequest, BookResponse } from './model';

@Injectable({ providedIn: 'root' })
export class BooksApi {
  private readonly http = inject(HttpClient);
  private readonly url = `${environment.apiUrl}/api/books`;

  list() {
    return this.http.get<BookResponse[]>(this.url);
  }

  create(book: BookRequest) {
    return this.http.post<BookResponse>(this.url, book);
  }

  update(id: string, book: BookRequest) {
    return this.http.put<BookResponse>(`${this.url}/${id}`, book);
  }

  remove(id: string) {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}

import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class App {
  private readonly http = inject(HttpClient);

  protected readonly apiStatus = signal('checking...');

  constructor() {
    this.http.get(`${environment.apiUrl}/health`, { responseType: 'text' }).subscribe({
      next: (value) => this.apiStatus.set(value),
      error: (err) => this.apiStatus.set(`error ${err.status}`),
    });
  }
}

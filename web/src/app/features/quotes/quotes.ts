import { DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  inject,
  signal,
  viewChild,
} from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { QuoteResponse } from '../../core/api/model';
import { QuotesApi } from '../../core/api/quotes-api';

@Component({
  selector: 'app-quotes',
  imports: [ReactiveFormsModule, DatePipe],
  templateUrl: './quotes.html',
  styleUrl: './quotes.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Quotes {
  private readonly fb = inject(NonNullableFormBuilder);
  private readonly api = inject(QuotesApi);
  private readonly dialog = viewChild.required<ElementRef<HTMLDialogElement>>('dialog');

  protected readonly items = signal<QuoteResponse[]>([]);
  protected readonly loading = signal(true);
  protected readonly saving = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly editingId = signal<string | null>(null);

  protected readonly form = this.fb.group({
    text: ['', [Validators.required, Validators.maxLength(1000)]],
    author: ['', [Validators.maxLength(200)]],
  });

  constructor() {
    this.load();
  }

  protected add(): void {
    this.editingId.set(null);
    this.form.reset();
    this.dialog().nativeElement.showModal();
  }

  protected edit(quote: QuoteResponse): void {
    this.editingId.set(quote.id);
    this.form.setValue({ text: quote.text, author: quote.author ?? '' });
    this.dialog().nativeElement.showModal();
  }

  protected cancel(): void {
    this.dialog().nativeElement.close();
    this.editingId.set(null);
    this.form.reset();
  }

  protected submit(): void {
    if (this.form.invalid) return;

    this.saving.set(true);
    this.error.set(null);

    const id = this.editingId();
    const { text, author } = this.form.getRawValue();
    const value = { text, author: author.trim() || null };

    (id ? this.api.update(id, value) : this.api.create(value)).subscribe({
      next: () => {
        this.saving.set(false);
        this.cancel();
        this.load();
      },
      error: () => {
        this.error.set('Save failed.');
        this.saving.set(false);
      },
    });
  }

  protected remove(quote: QuoteResponse): void {
    if (!confirm('Delete this quote?')) return;

    this.api.remove(quote.id).subscribe({
      next: () => this.load(),
      error: () => this.error.set('Delete failed.'),
    });
  }

  private load(): void {
    this.loading.set(true);
    this.error.set(null);
    this.api.list().subscribe({
      next: (quotes) => {
        this.items.set(quotes);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Could not load quotes.');
        this.loading.set(false);
      },
    });
  }
}

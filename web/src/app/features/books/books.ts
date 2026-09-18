import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  inject,
  signal,
  viewChild,
} from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { BooksApi } from '../../core/api/books-api';
import { BookResponse } from '../../core/api/model';

@Component({
  selector: 'app-books',
  imports: [ReactiveFormsModule],
  templateUrl: './books.html',
  styleUrl: './books.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Books {
  private readonly fb = inject(NonNullableFormBuilder);
  private readonly api = inject(BooksApi);
  private readonly dialog = viewChild.required<ElementRef<HTMLDialogElement>>('dialog');

  protected readonly items = signal<BookResponse[]>([]);
  protected readonly loading = signal(true);
  protected readonly saving = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly editingId = signal<string | null>(null);

  protected readonly form = this.fb.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    author: ['', [Validators.required, Validators.maxLength(200)]],
    publishedOn: ['', [Validators.required]],
  });

  constructor() {
    this.load();
  }

  protected add(): void {
    this.editingId.set(null);
    this.form.reset();
    this.dialog().nativeElement.showModal();
  }

  protected edit(book: BookResponse): void {
    this.editingId.set(book.id);
    this.form.setValue({
      title: book.title,
      author: book.author,
      publishedOn: book.publishedOn,
    });
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
    const value = this.form.getRawValue();

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

  protected remove(book: BookResponse): void {
    if (!confirm(`Delete "${book.title}"?`)) return;

    this.api.remove(book.id).subscribe({
      next: () => this.load(),
      error: () => this.error.set('Delete failed.'),
    });
  }

  private load(): void {
    this.error.set(null);
    this.loading.set(true);
    this.api.list().subscribe({
      next: (books) => {
        this.items.set(books);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Could not load books.');
        this.loading.set(false);
      },
    });
  }
}

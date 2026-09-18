import { Injectable, signal } from '@angular/core';

const THEME_KEY = 'bookapp.theme';

@Injectable({ providedIn: 'root' })
export class Theme {
  private readonly root = document.documentElement;
  private readonly _isDark = signal(this.root.dataset['bsTheme'] === 'dark');

  readonly isDark = this._isDark.asReadonly();

  toggle(): void {
    const isDark = !this._isDark();
    this._isDark.set(isDark);
    this.root.dataset['bsTheme'] = isDark ? 'dark' : 'light';
    localStorage.setItem(THEME_KEY, isDark ? 'dark' : 'light');
  }
}

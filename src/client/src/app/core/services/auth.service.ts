import { Injectable, computed, signal, PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Router } from '@angular/router';
import { User, AuthState, LoginCredentials, RegisterCredentials } from '../models/auth.model';

// ── Mock user database ────────────────────────────────────────────────────────
export const MOCK_USERS: Array<User & { password: string }> = [
  {
    id: 'admin-001',
    name: 'Admin User',
    email: 'admin@nexshop.com',
    password: 'admin123',
    role: 'admin',
    avatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=admin',
    createdAt: '2024-01-01T00:00:00Z',
  },
  {
    id: 'user-001',
    name: 'John Doe',
    email: 'john@example.com',
    password: 'password123',
    role: 'customer',
    avatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=john',
    createdAt: '2024-03-15T00:00:00Z',
  },
];

const STORAGE_KEY = 'nexshop_auth';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly platformId = inject(PLATFORM_ID);
  private readonly router = inject(Router);

  // ── State ───────────────────────────────────────────────────────────────────
  private readonly _state = signal<AuthState>(this._loadFromStorage());

  readonly currentUser = computed(() => this._state().user);
  readonly isLoggedIn = computed(() => this._state().isLoggedIn);
  readonly isAdmin = computed(() => this._state().user?.role === 'admin');
  readonly userName = computed(() => this._state().user?.name ?? '');
  readonly userAvatar = computed(() => this._state().user?.avatar ?? '');

  // ── Auth Methods ────────────────────────────────────────────────────────────
  login(credentials: LoginCredentials): { success: boolean; error?: string } {
    const match = MOCK_USERS.find(
      u => u.email.toLowerCase() === credentials.email.toLowerCase() && u.password === credentials.password
    );
    if (!match) {
      return { success: false, error: 'Invalid email or password. Try admin@nexshop.com / admin123' };
    }
    const { password, ...user } = match;
    const token = `mock-jwt-${user.id}-${Date.now()}`;
    this._setState({ user, token, isLoggedIn: true });
    return { success: true };
  }

  register(credentials: RegisterCredentials): { success: boolean; error?: string } {
    if (credentials.password !== credentials.confirmPassword) {
      return { success: false, error: 'Passwords do not match.' };
    }
    const exists = MOCK_USERS.find(u => u.email.toLowerCase() === credentials.email.toLowerCase());
    if (exists) {
      return { success: false, error: 'An account with this email already exists.' };
    }
    const newUser: User = {
      id: `user-${Date.now()}`,
      name: credentials.name,
      email: credentials.email,
      role: 'customer',
      avatar: `https://api.dicebear.com/7.x/avataaars/svg?seed=${credentials.name}`,
      createdAt: new Date().toISOString(),
    };
    MOCK_USERS.push({ ...newUser, password: credentials.password });
    const token = `mock-jwt-${newUser.id}-${Date.now()}`;
    this._setState({ user: newUser, token, isLoggedIn: true });
    return { success: true };
  }

  logout(): void {
    this._setState({ user: null, token: null, isLoggedIn: false });
    this.router.navigate(['/']);
  }

  // ── Storage ─────────────────────────────────────────────────────────────────
  private _setState(state: AuthState): void {
    this._state.set(state);
    if (isPlatformBrowser(this.platformId)) {
      if (state.isLoggedIn) {
        localStorage.setItem(STORAGE_KEY, JSON.stringify(state));
      } else {
        localStorage.removeItem(STORAGE_KEY);
      }
    }
  }

  private _loadFromStorage(): AuthState {
    const empty: AuthState = { user: null, token: null, isLoggedIn: false };
    if (!isPlatformBrowser(this.platformId)) return empty;
    try {
      const raw = localStorage.getItem(STORAGE_KEY);
      if (!raw) return empty;
      const parsed = JSON.parse(raw) as AuthState;
      return parsed.isLoggedIn && parsed.user ? parsed : empty;
    } catch {
      return empty;
    }
  }
}

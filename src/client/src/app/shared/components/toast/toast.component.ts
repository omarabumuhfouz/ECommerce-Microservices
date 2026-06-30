import { Component, inject } from '@angular/core';
import { ToastService } from '../../../core/services/toast.service';

@Component({
  selector: 'app-toast',
  standalone: true,
  template: `
    <div class="toast-container">
      @for (toast of toastService.toasts(); track toast.id) {
        <div class="toast toast-{{toast.type}}">
          <span class="toast-icon">{{toast.icon}}</span>
          <span class="toast-message">{{toast.message}}</span>
          <button class="toast-close" (click)="toastService.dismiss(toast.id)">&times;</button>
        </div>
      }
    </div>
  `,
  styles: [`
    .toast-container {
      position: fixed;
      bottom: 20px;
      right: 20px;
      z-index: 9999;
      display: flex;
      flex-direction: column;
      gap: 10px;
      pointer-events: none;
    }
    .toast {
      pointer-events: auto;
      min-width: 250px;
      max-width: 400px;
      background: var(--bg-card);
      border: 1px solid var(--border-color);
      border-radius: var(--border-radius-md);
      padding: 12px 16px;
      display: flex;
      align-items: center;
      gap: 12px;
      box-shadow: var(--shadow-md);
      animation: slideIn 0.3s ease forwards;
    }
    .toast-success { border-left: 4px solid var(--color-success); }
    .toast-error { border-left: 4px solid var(--color-error); }
    .toast-info { border-left: 4px solid var(--color-accent); }
    .toast-warning { border-left: 4px solid var(--color-warning); }
    
    .toast-icon { font-size: 1.25rem; }
    .toast-message { flex: 1; font-size: 0.875rem; color: var(--text-primary); }
    .toast-close {
      background: none;
      border: none;
      color: var(--text-muted);
      font-size: 1.5rem;
      cursor: pointer;
      line-height: 1;
    }
    .toast-close:hover { color: var(--text-primary); }
    
    @keyframes slideIn {
      from { transform: translateX(100%); opacity: 0; }
      to { transform: translateX(0); opacity: 1; }
    }
  `]
})
export class ToastComponent {
  toastService = inject(ToastService);
}

import { Component, inject, computed, signal, ChangeDetectionStrategy } from '@angular/core';
import { DatePipe, TitleCasePipe } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../../../../core/services/user.service';
import { ToastService } from '../../../../core/services/toast.service';

@Component({
  selector: 'app-user-management',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [DatePipe, TitleCasePipe, ReactiveFormsModule],
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css'
})
export class UserManagementComponent {
  private userService = inject(UserService);
  private toastService = inject(ToastService);
  private fb = inject(FormBuilder);

  users = this.userService.users;

  // Selection state
  selectedUserIds = signal<Set<string>>(new Set());
  
  // Dashboard Stats
  totalUsers = computed(() => this.users().length);
  adminUsers = computed(() => this.users().filter(u => u.role === 'admin').length);
  newUsersThisWeek = computed(() => {
    const oneWeekAgo = new Date(Date.now() - 7 * 24 * 60 * 60 * 1000);
    return this.users().filter(u => new Date(u.createdAt) >= oneWeekAgo).length;
  });

  // Email form
  isEmailModalOpen = signal(false);
  isSending = signal(false);
  
  emailForm = this.fb.nonNullable.group({
    subject: ['', [Validators.required, Validators.minLength(3)]],
    message: ['', [Validators.required, Validators.minLength(10)]]
  });

  // Checkbox helpers
  get allSelected(): boolean {
    return this.users().length > 0 && this.selectedUserIds().size === this.users().length;
  }

  get someSelected(): boolean {
    return this.selectedUserIds().size > 0 && this.selectedUserIds().size < this.users().length;
  }

  toggleAll(event: Event) {
    const isChecked = (event.target as HTMLInputElement).checked;
    if (isChecked) {
      this.selectedUserIds.set(new Set(this.users().map(u => u.id)));
    } else {
      this.selectedUserIds.set(new Set());
    }
  }

  toggleUser(userId: string, event: Event) {
    const isChecked = (event.target as HTMLInputElement).checked;
    this.selectedUserIds.update(set => {
      const newSet = new Set(set);
      if (isChecked) {
        newSet.add(userId);
      } else {
        newSet.delete(userId);
      }
      return newSet;
    });
  }

  isSelected(userId: string): boolean {
    return this.selectedUserIds().has(userId);
  }

  openEmailModal() {
    if (this.selectedUserIds().size > 0) {
      this.isEmailModalOpen.set(true);
    }
  }

  closeEmailModal() {
    this.isEmailModalOpen.set(false);
    if (!this.isSending()) {
      this.emailForm.reset();
    }
  }

  async sendBulkEmail() {
    if (this.emailForm.invalid || this.selectedUserIds().size === 0) return;

    this.isSending.set(true);
    const { subject, message } = this.emailForm.getRawValue();
    const ids = Array.from(this.selectedUserIds());

    try {
      await this.userService.sendEmail(ids, subject, message);
      this.toastService.success(`Email successfully sent to ${ids.length} user(s)!`);
      this.emailForm.reset();
      this.selectedUserIds.set(new Set()); // clear selection
      this.closeEmailModal();
    } catch (err) {
      this.toastService.error('Failed to send email. Please try again.');
    } finally {
      this.isSending.set(false);
    }
  }
}

import { Injectable, signal, computed } from '@angular/core';
import { User } from '../models/auth.model';
import { MOCK_USERS } from './auth.service';

@Injectable({ providedIn: 'root' })
export class UserService {
  // Extract just the user data (remove passwords) for the admin view
  private readonly _users = signal<User[]>(
    MOCK_USERS.map(({ password, ...user }) => user)
  );

  readonly users = this._users.asReadonly();

  // Simulate an email sending microservice
  async sendEmail(userIds: string[], subject: string, message: string): Promise<boolean> {
    // Simulate network delay
    await new Promise(resolve => setTimeout(resolve, 800));
    
    // In a real app, this would be an HTTP POST request:
    // return this.http.post('/api/users/email', { userIds, subject, message }).toPromise();
    
    console.log(`[Email Service] Sent to ${userIds.length} users. Subject: ${subject}`);
    return true;
  }
}

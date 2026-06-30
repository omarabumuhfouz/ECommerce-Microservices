import { Component, inject, computed } from '@angular/core';
import { TitleCasePipe, CurrencyPipe, DatePipe } from '@angular/common';
import { AuthService } from '../../core/services/auth.service';
import { OrderService } from '../../core/services/order.service';
import { CategoryManagementComponent } from './components/category-management/category-management.component';
import { InventoryManagementComponent } from './components/inventory-management/inventory-management.component';
import { OrderManagementComponent } from './components/order-management/order-management.component';
import { UserManagementComponent } from './components/user-management/user-management.component';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [TitleCasePipe, CurrencyPipe, DatePipe, CategoryManagementComponent, InventoryManagementComponent, OrderManagementComponent, UserManagementComponent],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css'
})
export class AdminComponent {
  authService = inject(AuthService);

  activeTab: 'dashboard' | 'products' | 'categories' | 'inventory' | 'orders' | 'users' = 'dashboard';

  stats = [
    { label: 'Total Revenue', value: '$124,563.00', trend: '+14%', isPositive: true },
    { label: 'Orders', value: '1,234', trend: '+5%', isPositive: true },
    { label: 'Products', value: '45', trend: '0%', isPositive: true },
    { label: 'Active Users', value: '892', trend: '-2%', isPositive: false }
  ];

  private orderService = inject(OrderService);
  recentOrders = computed(() => this.orderService.orders().slice(0, 5));

  setTab(tab: 'dashboard' | 'products' | 'categories' | 'inventory' | 'orders' | 'users') {
    this.activeTab = tab;
  }
}

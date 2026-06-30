import { Component, inject, computed, ChangeDetectionStrategy, signal } from '@angular/core';
import { CurrencyPipe, DatePipe, NgClass, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OrderService } from '../../../../core/services/order.service';
import { ToastService } from '../../../../core/services/toast.service';
import { Order, OrderStatus } from '../../../../core/models/order.model';

@Component({
  selector: 'app-order-management',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CurrencyPipe, DatePipe, NgClass, NgIf, FormsModule],
  templateUrl: './order-management.component.html',
  styleUrl: './order-management.component.css'
})
export class OrderManagementComponent {
  private orderService = inject(OrderService);
  private toastService = inject(ToastService);

  orders = this.orderService.orders;
  searchQuery = signal('');

  filteredOrders = computed(() => {
    const q = this.searchQuery().toLowerCase();
    const all = this.orders();
    if (!q) return all;
    return all.filter(o => 
      o.id.toLowerCase().includes(q) || 
      o.customerName.toLowerCase().includes(q) || 
      o.customerEmail.toLowerCase().includes(q)
    );
  });

  // Available statuses for the dropdown
  statuses: OrderStatus[] = ['Pending', 'Processing', 'Shipped', 'Delivered', 'Cancelled'];

  // Dashboard Stats
  totalOrders = computed(() => this.orders().length);
  pendingOrders = computed(() => this.orders().filter(o => o.status === 'Pending' || o.status === 'Processing').length);
  totalRevenue = computed(() => this.orders()
    .filter(o => o.status !== 'Cancelled')
    .reduce((sum, o) => sum + o.total, 0)
  );

  updateStatus(order: Order, event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    const newStatus = selectElement.value as OrderStatus;
    
    if (newStatus !== order.status) {
      this.orderService.updateOrderStatus(order.id, newStatus);
      this.toastService.success(`Order ${order.id} marked as ${newStatus}`);
    }
  }

  getStatusClass(status: OrderStatus): string {
    switch (status) {
      case 'Pending': return 'status-pending';
      case 'Processing': return 'status-processing';
      case 'Shipped': return 'status-shipped';
      case 'Delivered': return 'status-delivered';
      case 'Cancelled': return 'status-cancelled';
      default: return '';
    }
  }
}

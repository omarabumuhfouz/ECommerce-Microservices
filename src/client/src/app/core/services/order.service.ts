import { Injectable, signal, computed } from '@angular/core';
import { Order, OrderStatus } from '../models/order.model';

const MOCK_ORDERS: Order[] = [
  {
    id: 'ORD-1001',
    customerId: 'user-001',
    customerName: 'Alice Brown',
    customerEmail: 'alice@example.com',
    items: [
      { productId: 'p1', productName: 'ProMax Ultra X Pro', quantity: 1, price: 1199, thumbnail: 'https://picsum.photos/seed/phone1/600/600' }
    ],
    total: 1199,
    status: 'Pending',
    shippingAddress: '123 Tech Avenue, Silicon Valley, CA 94025',
    createdAt: new Date(Date.now() - 1000 * 60 * 60 * 2).toISOString() // 2 hours ago
  },
  {
    id: 'ORD-1002',
    customerId: 'user-002',
    customerName: 'Jane Smith',
    customerEmail: 'jane.smith@example.com',
    items: [
      { productId: 'p5', productName: 'NexBook Pro 16', quantity: 1, price: 2499, thumbnail: 'https://picsum.photos/seed/laptop1/600/600' },
      { productId: 'p8', productName: 'BubblePods Pro', quantity: 1, price: 249, thumbnail: 'https://picsum.photos/seed/earbuds1/600/600' }
    ],
    total: 2748,
    status: 'Processing',
    shippingAddress: '456 Innovation Blvd, Austin, TX 73301',
    createdAt: new Date(Date.now() - 1000 * 60 * 60 * 24).toISOString() // 1 day ago
  },
  {
    id: 'ORD-1003',
    customerId: 'user-003',
    customerName: 'Bob Johnson',
    customerEmail: 'bob.j@example.com',
    items: [
      { productId: 'p11', productName: 'Velocity Slim Tee', quantity: 2, price: 39, thumbnail: 'https://picsum.photos/seed/shirt1/600/600' }
    ],
    total: 78,
    status: 'Shipped',
    shippingAddress: '789 Commerce St, New York, NY 10001',
    createdAt: new Date(Date.now() - 1000 * 60 * 60 * 72).toISOString() // 3 days ago
  },
  {
    id: 'ORD-1004',
    customerId: 'user-004',
    customerName: 'John Doe',
    customerEmail: 'john.doe@example.com',
    items: [
      { productId: 'p15', productName: 'Velvet Cloud Sofa', quantity: 1, price: 1299, thumbnail: 'https://picsum.photos/seed/sofa1/600/600' }
    ],
    total: 1299,
    status: 'Delivered',
    shippingAddress: '321 Maple Lane, Seattle, WA 98101',
    createdAt: new Date(Date.now() - 1000 * 60 * 60 * 168).toISOString() // 7 days ago
  },
  {
    id: 'ORD-1005',
    customerId: 'user-005',
    customerName: 'Emily Clark',
    customerEmail: 'emily.c@example.com',
    items: [
      { productId: 'p18', productName: 'PowerFlex Yoga Mat', quantity: 1, price: 69, thumbnail: 'https://picsum.photos/seed/yoga1/600/600' }
    ],
    total: 69,
    status: 'Cancelled',
    shippingAddress: '555 Fitness Way, Denver, CO 80202',
    createdAt: new Date(Date.now() - 1000 * 60 * 60 * 48).toISOString() // 2 days ago
  }
];

@Injectable({ providedIn: 'root' })
export class OrderService {
  private readonly _orders = signal<Order[]>(MOCK_ORDERS);

  readonly orders = this._orders.asReadonly();

  updateOrderStatus(orderId: string, status: OrderStatus): void {
    this._orders.update(orders =>
      orders.map(o => o.id === orderId ? { ...o, status } : o)
    );
  }
}

import { Injectable, computed, signal } from '@angular/core';
import { Cart, CartItem } from '../models/cart.model';
import { Product } from '../models/product.model';

@Injectable({ providedIn: 'root' })
export class CartService {
  private readonly _items = signal<CartItem[]>([]);

  readonly items = this._items.asReadonly();

  readonly totalItems = computed(() => this._items().reduce((sum, i) => sum + i.quantity, 0));

  readonly subtotal = computed(() =>
    this._items().reduce((sum, i) => sum + i.product.price * i.quantity, 0)
  );

  readonly discount = computed(() =>
    this._items().reduce((sum, i) => {
      const orig = i.product.originalPrice ?? i.product.price;
      return sum + (orig - i.product.price) * i.quantity;
    }, 0)
  );

  readonly total = computed(() => this.subtotal());

  readonly cart = computed<Cart>(() => ({
    items: this._items(),
    totalItems: this.totalItems(),
    subtotal: this.subtotal(),
    discount: this.discount(),
    total: this.total(),
  }));

  addItem(product: Product, quantity = 1, selectedAttributes: Record<string, string> = {}): void {
    this._items.update(items => {
      const existing = items.find(i => i.product.id === product.id);
      if (existing) {
        return items.map(i =>
          i.product.id === product.id ? { ...i, quantity: i.quantity + quantity } : i
        );
      }
      return [...items, { product, quantity, selectedAttributes }];
    });
  }

  removeItem(productId: string): void {
    this._items.update(items => items.filter(i => i.product.id !== productId));
  }

  updateQuantity(productId: string, quantity: number): void {
    if (quantity <= 0) { this.removeItem(productId); return; }
    this._items.update(items =>
      items.map(i => i.product.id === productId ? { ...i, quantity } : i)
    );
  }

  clear(): void { this._items.set([]); }

  isInCart(productId: string): boolean {
    return this._items().some(i => i.product.id === productId);
  }
}

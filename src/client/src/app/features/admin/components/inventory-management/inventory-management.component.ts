import { Component, inject, computed, ChangeDetectionStrategy, signal } from '@angular/core';
import { CurrencyPipe, TitleCasePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../../../core/services/product.service';
import { ToastService } from '../../../../core/services/toast.service';
import { Product } from '../../../../core/models/product.model';

@Component({
  selector: 'app-inventory-management',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CurrencyPipe, TitleCasePipe, FormsModule],
  templateUrl: './inventory-management.component.html',
  styleUrl: './inventory-management.component.css'
})
export class InventoryManagementComponent {
  private productService = inject(ProductService);
  private toastService = inject(ToastService);

  searchQuery = signal('');

  // Use a computed to filter products if there is a search query
  products = computed(() => {
    const q = this.searchQuery().toLowerCase();
    const all = this.productService.products();
    if (!q) return all;
    return all.filter(p => p.name.toLowerCase().includes(q) || p.id.toLowerCase().includes(q));
  });

  // Dashboard Stats
  totalItems = computed(() => this.products().length);
  lowStockItems = computed(() => this.products().filter(p => p.stock > 0 && p.stock < 10).length);
  outOfStockItems = computed(() => this.products().filter(p => p.stock === 0).length);
  totalValue = computed(() => this.products().reduce((sum, p) => sum + (p.price * p.stock), 0));

  updateStock(product: Product, change: number) {
    const newStock = product.stock + change;
    this.productService.updateStock(product.id, newStock);
    if (newStock === 0) {
      this.toastService.warning(`"${product.name}" is now out of stock.`);
    } else {
      this.toastService.success(`Stock updated for "${product.name}".`);
    }
  }

  setStock(product: Product, event: Event) {
    const input = event.target as HTMLInputElement;
    const value = parseInt(input.value, 10);
    if (isNaN(value) || value < 0) {
      input.value = product.stock.toString(); // Revert on invalid
      return;
    }
    if (value !== product.stock) {
      this.productService.updateStock(product.id, value);
      this.toastService.success(`Stock set to ${value} for "${product.name}".`);
    }
  }
}

import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CartService } from '../../core/services/cart.service';
import { CurrencyPipe, KeyValuePipe } from '@angular/common';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [RouterLink, CurrencyPipe, KeyValuePipe],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent {
  cartService = inject(CartService);

  updateQuantity(productId: string, currentQty: number, change: number) {
    const newQty = currentQty + change;
    if (newQty > 0) {
      this.cartService.updateQuantity(productId, newQty);
    } else {
      this.cartService.removeItem(productId);
    }
  }

  removeItem(productId: string) {
    this.cartService.removeItem(productId);
  }

  checkout() {
    alert('Checkout flow coming soon!');
  }
}

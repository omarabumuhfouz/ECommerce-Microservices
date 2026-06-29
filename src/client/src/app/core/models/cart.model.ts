import { Product } from './product.model';

export interface CartItem {
  product: Product;
  quantity: number;
  selectedAttributes: Record<string, string>;
}

export interface Cart {
  items: CartItem[];
  totalItems: number;
  subtotal: number;
  discount: number;
  total: number;
}

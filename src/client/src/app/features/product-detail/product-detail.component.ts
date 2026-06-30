import { Component, ChangeDetectionStrategy, inject, signal, computed, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CurrencyPipe } from '@angular/common';
import { ProductService } from '../../core/services/product.service';
import { CategoryService } from '../../core/services/category.service';
import { CartService } from '../../core/services/cart.service';
import { StarRatingComponent } from '../../shared/components/star-rating/star-rating.component';
import { ProductCardComponent } from '../../shared/components/product-card/product-card.component';
import { ProductFeedbackComponent } from '../../shared/components/product-feedback/product-feedback.component';
import { Product, ProductReview } from '../../core/models/product.model';
import { Category } from '../../core/models/category.model';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, CurrencyPipe, StarRatingComponent, ProductCardComponent, ProductFeedbackComponent],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.css',
})
export class ProductDetailComponent implements OnInit {
  readonly Math = Math;
  private route = inject(ActivatedRoute);
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);
  private cartService = inject(CartService);

  product = signal<Product | null>(null);
  category = signal<Category | null>(null);
  breadcrumb = signal<Category[]>([]);
  relatedProducts = signal<Product[]>([]);
  selectedImage = signal(0);
  quantity = signal(1);
  selectedAttrs = signal<Record<string, string>>({});
  addedToCart = signal(false);
  reviews = signal<ProductReview[]>([]);

  readonly averageRating = computed(() => {
    const revs = this.reviews();
    if (!revs.length) return this.product()?.rating ?? 0;
    const sum = revs.reduce((acc, r) => acc + r.rating, 0);
    return Number((sum / revs.length).toFixed(1));
  });

  readonly isAvailable = computed(() => {
    const p = this.product();
    if (!p) return false;
    return p.inStock ?? (p.stock > 0);
  });

  readonly inCart = computed(() => {
    const p = this.product();
    return p ? this.cartService.isInCart(p.id) : false;
  });

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const slug = params.get('slug');
      if (!slug) return;
      const product = this.productService.getBySlug(slug);
      if (!product) return;
      this.product.set(product);
      this.selectedImage.set(0);
      this.quantity.set(1);
      this.addedToCart.set(false);
      const cat = this.categoryService.getById(product.categoryId);
      this.category.set(cat ?? null);
      if (cat) this.breadcrumb.set(this.categoryService.getCategoryPath(cat.id));
      this.relatedProducts.set(this.productService.getRelated(product));
      this.reviews.set(this.productService.getReviews(product.id));
    });
  }

  addToCart() {
    const p = this.product();
    if (!p) return;
    this.cartService.addItem(p, this.quantity(), this.selectedAttrs());
    this.addedToCart.set(true);
    setTimeout(() => this.addedToCart.set(false), 2000);
  }

  selectAttr(key: string, value: string) {
    this.selectedAttrs.update(a => ({ ...a, [key]: value }));
  }

  isAttrSelected(key: string, value: string): boolean {
    return this.selectedAttrs()[key] === value;
  }

  getAttrKeys(attrs: Record<string, string | string[]>): string[] {
    return Object.keys(attrs).filter(k => Array.isArray(attrs[k]));
  }

  getAttrValues(attrs: Record<string, string | string[]>, key: string): string[] {
    const val = attrs[key];
    return Array.isArray(val) ? val : [val];
  }
}

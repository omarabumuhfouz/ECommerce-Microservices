import { Component, ChangeDetectionStrategy, inject, signal, HostListener, computed } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CategoryService } from '../../../core/services/category.service';
import { CartService } from '../../../core/services/cart.service';
import { CategoryTree } from '../../../core/models/category.model';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, RouterLinkActive, FormsModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {
  private categoryService = inject(CategoryService);
  private cartService = inject(CartService);
  private router = inject(Router);
  public authService = inject(AuthService);

  readonly categoryTree = this.categoryService.categoryTree;
  readonly cartCount = this.cartService.totalItems;

  searchQuery = signal('');
  isMobileMenuOpen = signal(false);
  activeMenuCategory = signal<CategoryTree | null>(null);
  isScrolled = signal(false);

  @HostListener('window:scroll')
  onScroll() { this.isScrolled.set(window.scrollY > 20); }

  toggleMobileMenu() { this.isMobileMenuOpen.update(v => !v); }
  closeMobileMenu() { this.isMobileMenuOpen.set(false); }

  openMegaMenu(category: CategoryTree) { this.activeMenuCategory.set(category); }
  closeMegaMenu() { this.activeMenuCategory.set(null); }

  onSearch(e: Event) {
    e.preventDefault();
    const q = this.searchQuery();
    if (q.trim()) {
      this.router.navigate(['/products'], { queryParams: { search: q } });
      this.searchQuery.set('');
    }
  }
}

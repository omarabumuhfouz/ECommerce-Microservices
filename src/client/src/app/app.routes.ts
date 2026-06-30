import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./features/home/home.component').then(m => m.HomeComponent),
    title: 'NexShop — Premium E-Commerce',
  },
  {
    path: 'products',
    loadComponent: () => import('./features/products/products.component').then(m => m.ProductsComponent),
    title: 'All Products — NexShop',
  },
  {
    path: 'product/:slug',
    loadComponent: () => import('./features/product-detail/product-detail.component').then(m => m.ProductDetailComponent),
    title: 'Product — NexShop',
  },
  {
    path: 'category/:root',
    loadComponent: () => import('./features/category/category.component').then(m => m.CategoryComponent),
  },
  {
    path: 'category/:root/:sub',
    loadComponent: () => import('./features/category/category.component').then(m => m.CategoryComponent),
  },
  {
    path: 'category/:root/:sub/:child',
    loadComponent: () => import('./features/category/category.component').then(m => m.CategoryComponent),
  },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent),
    title: 'Log In — NexShop',
  },
  {
    path: 'register',
    loadComponent: () => import('./features/auth/register/register.component').then(m => m.RegisterComponent),
    title: 'Register — NexShop',
  },
  {
    path: 'admin',
    loadComponent: () => import('./features/admin/admin.component').then(m => m.AdminComponent),
    canActivate: [() => import('./core/guards/admin.guard').then(m => m.adminGuard)],
    title: 'Admin Dashboard — NexShop',
  },
  {
    path: 'cart',
    loadComponent: () => import('./features/cart/cart.component').then(m => m.CartComponent),
    title: 'Shopping Cart — NexShop',
  },
  {
    path: 'profile',
    loadComponent: () => import('./features/profile/profile.component').then(m => m.ProfileComponent),
    canActivate: [() => import('./core/guards/auth.guard').then(m => m.authGuard)],
    title: 'My Profile — NexShop',
  },
  { path: '**', redirectTo: '' },
];

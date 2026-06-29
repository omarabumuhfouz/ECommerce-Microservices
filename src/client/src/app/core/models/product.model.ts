export interface Product {
  id: string;
  name: string;
  slug: string;
  description: string;
  shortDescription: string;
  price: number;
  originalPrice?: number;
  discount?: number;
  images: string[];
  thumbnail: string;
  categoryId: string;
  brand: string;
  rating: number;
  reviewCount: number;
  stock: number;
  inStock?: boolean;
  tags: string[];
  attributes: Record<string, string | string[]>;
  isFeatured: boolean;
  isNew: boolean;
  isOnSale: boolean;
  createdAt: string;
}

export interface ProductReview {
  id: string;
  productId: string;
  author: string;
  avatar: string;
  rating: number;
  title: string;
  body: string;
  date: string;
  helpful: number;
}

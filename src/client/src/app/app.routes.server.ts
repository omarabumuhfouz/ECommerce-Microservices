import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  { path: '', renderMode: RenderMode.Prerender },
  { path: 'products', renderMode: RenderMode.Server },
  { path: 'product/:slug', renderMode: RenderMode.Server },
  { path: 'category/:root', renderMode: RenderMode.Server },
  { path: 'category/:root/:sub', renderMode: RenderMode.Server },
  { path: 'category/:root/:sub/:child', renderMode: RenderMode.Server },
  { path: '**', renderMode: RenderMode.Server },
];

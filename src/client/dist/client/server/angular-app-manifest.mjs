
export default {
  bootstrap: () => import('./main.server.mjs').then(m => m.default),
  inlineCriticalCss: true,
  baseHref: '/',
  locale: undefined,
  routes: [
  {
    "renderMode": 2,
    "preload": [
      "chunk-VY7YYIAP.js",
      "chunk-5S3DB2CU.js"
    ],
    "route": "/"
  },
  {
    "renderMode": 0,
    "preload": [
      "chunk-LO7TAXBY.js",
      "chunk-5S3DB2CU.js"
    ],
    "route": "/products"
  },
  {
    "renderMode": 0,
    "preload": [
      "chunk-NNV2AFAY.js",
      "chunk-5S3DB2CU.js"
    ],
    "route": "/product/*"
  },
  {
    "renderMode": 0,
    "preload": [
      "chunk-4MI67PTN.js",
      "chunk-5S3DB2CU.js"
    ],
    "route": "/category/*"
  },
  {
    "renderMode": 0,
    "preload": [
      "chunk-4MI67PTN.js",
      "chunk-5S3DB2CU.js"
    ],
    "route": "/category/*/*"
  },
  {
    "renderMode": 0,
    "preload": [
      "chunk-4MI67PTN.js",
      "chunk-5S3DB2CU.js"
    ],
    "route": "/category/*/*/*"
  },
  {
    "renderMode": 0,
    "redirectTo": "/",
    "route": "/**"
  }
],
  entryPointToBrowserMapping: undefined,
  assets: {
    'index.csr.html': {size: 21523, hash: '47119e5362827cc69f70f208be47ab958c3b521ddf5b796236f7d9f974c23935', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1537, hash: '4547723873bef27a4fe671826e8bb385ab24ed32847f3388ac41e67ac23962a5', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'index.html': {size: 79854, hash: '875948bfc7cd5408a2ed7a0f62164636e9c86d8278868b06f2c2f452b463f2fb', text: () => import('./assets-chunks/index_html.mjs').then(m => m.default)},
    'styles-RAKP6LEA.css': {size: 20870, hash: 'ls0Xu8hxwGs', text: () => import('./assets-chunks/styles-RAKP6LEA_css.mjs').then(m => m.default)}
  },
};

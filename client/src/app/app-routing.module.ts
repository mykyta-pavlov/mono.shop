import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  // localhost:4200/ – call 'HomeComponent'
  {path: '', component: HomeComponent, data: {breadcrumb: 'Головна'}},
  {path: 'test-error', component: TestErrorComponent, data: {breadcrumb: 'Test Errors'}},
  {path: 'server-error', component: ServerErrorComponent, data: {breadcrumb: 'Server Error'}},
  {path: 'not-found', component: NotFoundComponent, data: {breadcrumb: 'Не знайдено'}},
  // localhost:4200/shop – call 'ShopComponent' etc.
  {path: 'shop', loadChildren: () => import('./shop/shop.module').then(mod => mod.ShopModule),
    data: {breadcrumb: 'Товари'}},
  {path: 'basket', loadChildren: () => import('./basket/basket.module').then(mod => mod.BasketModule),
    data: {breadcrumb: 'Кошик'}},
  {path: 'checkout', canActivate: [AuthGuard], loadChildren: () => import('./checkout/checkout.module').then(mod => mod.CheckoutModule),
    data: {breadcrumb: 'Оформлення замовлення'}},
  {path: 'orders', canActivate: [AuthGuard], loadChildren: () => import('./orders/orders.module').then(mod => mod.OrdersModule),
    data: {breadcrumb: 'Мої замовлення'}},
  {path: 'account', loadChildren: () => import('./account/account.module').then(mod => mod.AccountModule),
    data: {breadcrumb: {skip: true}}},
  {path: '**', redirectTo: 'not-found', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }

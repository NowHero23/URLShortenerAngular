import { HttpClient } from '@angular/common/http';
import { Component, effect, inject, resource } from '@angular/core';
import { UrlItem } from '../../../models/url-item.model';

@Component({
  selector: 'app-urls-table',
  imports: [],
  templateUrl: './urls-table.component.html',
  styleUrl: './urls-table.component.css',
})
export class UrlsTableComponent {
  http = inject(HttpClient);

  constructor() {
    // effect(() => {
    //   console.log('Value: ', this.urlsResource.value);
    //   console.log('Status: ', this.urlsResource.status);
    //   console.log('error: ', this.urlsResource.error);
    // });
  }
  urlsResource = resource({
    loader: () => {
      //`https://localhost:7048/api/ShortUrls/table`
      return fetch(`/api/ShortUrls/table`).then(
        (res) => res.json() as Promise<UrlItem[]>
      );
    },
  });
  refreshResource() {
    this.urlsResource.reload();
  }
}

import {LitElement, css, html} from 'lit';
import {customElement, property} from 'lit/decorators.js';
import { ref,Ref, createRef } from 'lit/directives/ref.js';

@customElement('nnp-input')
export class NnpInput extends LitElement {
  // Define scoped styles right with your component, in plain CSS
  static styles = css`
    :host {
      color: blue;
    }
  `;

  @property({type: String}) id: string = 'input-id';
  @property({type: String}) name?: string;
  @property({type: String}) label: string;
  @property({type: String}) type: string = "text";

  private notchMiddleRef: Ref<HTMLDivElement> = createRef();
  private labelRef: Ref<HTMLLabelElement> = createRef();

  connectedCallback() {
    super.connectedCallback()
    this.notchMiddleRef.value.style.width = `${this.labelRef.value.offsetWidth + 10}px`;
  }

  render() {
    return html`
      <div class="input-wrapper">
        <label 
          ref="labelRef"
          for="${this.id}"
          class="label"
        >
          ${this.label}
        </label>
        <input 
          id="${this.id}"
          name="${this.name || this.id}"
          class="input"
          type="${this.type}"
          value="modelValue"
          aria-labelledby="${this.id}"
          @input="$emit('update:modelValue', ($event.target as HTMLInputElement).value)"
        />
        <div class="notch">
          <div class="notch-start"></div>
          <div ref="notchMiddleRef" class="notch-middle"></div>
          <div class="notch-end"></div>
        </div>
      </div>
    `;
  }
}
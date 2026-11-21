"""
Makes the message cards for the Chemistry Molecule Builder lens (lens 009).

Each message becomes a PNG with a rounded navy card and bold white lettering,
so the words stay clear and crisp on any background.

Run it from this folder:   python make-molecule-cards.py
Change MESSAGES below to say something else, then run it again.
Needs Python 3 and Pillow (pip install pillow).
The font is Arial Rounded MT Bold. Use --font path/to/font.ttf for another.
"""
import argparse
import os
from PIL import Image, ImageDraw, ImageFont

# file name, first line (big), second line (small, gold)
# In the first line, "_2" makes a small number lowered like a chemistry subscript (H_2O)
MESSAGES = [
    ("message-1", "Oxygen (O)", "Tap to add hydrogen"),
    ("message-2", "Hydrogen (H)", "One more!"),
    ("message-3", "Water, H_2O", "Tap for the next molecule"),
    ("message-4", "Carbon (C)", "Tap to add oxygen"),
    ("message-5", "Oxygen (O)", "One more!"),
    ("message-6", "Carbon dioxide, CO_2", "Tap to start again"),
    ("message-0", "Build a molecule", "Tap to add an atom"),
]
NAVY = (22, 28, 70)
CYAN = (90, 220, 255)
GOLD = (255, 215, 120)
WHITE = (255, 255, 255)
SCALE = 2


def find_font(path):
    for p in [path, "C:/Windows/Fonts/ARLRDBD.TTF", "/System/Library/Fonts/Supplemental/Arial Rounded Bold.ttf"]:
        if p and os.path.exists(p):
            return p
    raise SystemExit("Font not found. Pass one with --font path/to/font.ttf")


def draw_formula(d, cx, cy, text, font, sub_font, fill):
    """Draws text centred on (cx, cy). A "_" followed by a digit is drawn small and lowered."""
    parts = []
    i = 0
    while i < len(text):
        if text[i] == "_" and i + 1 < len(text):
            parts.append((text[i + 1], True))
            i += 2
        else:
            parts.append((text[i], False))
            i += 1
    widths = [d.textlength(ch, font=sub_font if sub else font) for ch, sub in parts]
    x = cx - sum(widths) / 2
    for (ch, sub), w in zip(parts, widths):
        f = sub_font if sub else font
        d.text((x, cy + (22 * SCALE if sub else 0)), ch, font=f, fill=fill, anchor="lm",
               stroke_width=2 * SCALE, stroke_fill=(0, 0, 0, 90))
        x += w


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--font", default=None)
    font = find_font(ap.parse_args().font)
    here = os.path.dirname(os.path.abspath(__file__))
    big = ImageFont.truetype(font, 82 * SCALE)
    small = ImageFont.truetype(font, 54 * SCALE)
    sub = ImageFont.truetype(font, 52 * SCALE)
    W, H = 1100, 260
    for name, line1, line2 in MESSAGES:
        w, h = W * SCALE, H * SCALE
        img = Image.new("RGBA", (w, h), (0, 0, 0, 0))
        d = ImageDraw.Draw(img)
        m = 6 * SCALE
        d.rounded_rectangle((m, m, w - m, h - m), 60 * SCALE, fill=NAVY, outline=CYAN, width=8 * SCALE)
        shine = Image.new("RGBA", (w, h), (0, 0, 0, 0))
        ImageDraw.Draw(shine).rounded_rectangle((m + 14 * SCALE, m + 10 * SCALE, w - m - 14 * SCALE, h * 0.38), 30 * SCALE, fill=(255, 255, 255, 34))
        img = Image.alpha_composite(img, shine)
        d = ImageDraw.Draw(img)
        draw_formula(d, w // 2, 100 * SCALE, line1, big, sub, WHITE)
        d.text((w // 2, 190 * SCALE), line2, font=small, fill=GOLD, anchor="mm", stroke_width=1 * SCALE, stroke_fill=(0, 0, 0, 90))
        img.resize((W, H), Image.LANCZOS).save(os.path.join(here, name + ".png"))
    print("Made", len(MESSAGES), "pictures in", here)


if __name__ == "__main__":
    main()

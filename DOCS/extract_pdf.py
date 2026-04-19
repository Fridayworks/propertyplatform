from pdfminer.high_level import extract_text

pdf_path = r"C:\Users\Carso Thinkpad\.gemini\antigravity\scratch\PropertyPlatform\DOCS\SOFTWARE REQUIREMENTS SPECIFICATION (SRS), TSS & Business Plan.pdf"
out_path = r"C:\Users\Carso Thinkpad\.gemini\antigravity\scratch\PropertyPlatform\DOCS\srs_extracted.txt"

text = extract_text(pdf_path)
with open(out_path, "w", encoding="utf-8") as f:
    f.write(text)

print(f"Done — extracted {len(text)} characters")

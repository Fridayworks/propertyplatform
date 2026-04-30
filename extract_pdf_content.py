import pdfplumber
import os

def extract_text_from_pdf(pdf_path, output_md_path):
    """Extract text from PDF and save as markdown file"""
    try:
        text_content = ""
        
        with pdfplumber.open(pdf_path) as pdf:
            for page in pdf.pages:
                page_text = page.extract_text()
                if page_text:
                    text_content += page_text + "\n\n"
        
        # Write to markdown file
        with open(output_md_path, 'w', encoding='utf-8') as md_file:
            md_file.write(f"# {os.path.basename(pdf_path).replace('.pdf', '')}\n\n")
            md_file.write(text_content)
        
        print(f"Successfully extracted text from {pdf_path} to {output_md_path}")
        return True
        
    except Exception as e:
        print(f"Error extracting text from {pdf_path}: {str(e)}")
        return False

def main():
    # Define the PDF files to extract
    pdf_files = [
        "DOCS\\Master Baseline Specification Document.pdf",
        "DOCS\\CR4 FORMAL CHANGE REQUEST DOCUMENT.pdf"
    ]
    
    # Process each PDF file
    for pdf_file in pdf_files:
        # Use absolute path
        pdf_path = os.path.join(os.getcwd(), pdf_file)
        if os.path.exists(pdf_path):
            output_md_path = pdf_file.replace('.pdf', '.md')
            extract_text_from_pdf(pdf_path, output_md_path)
        else:
            print(f"PDF file not found: {pdf_path}")

if __name__ == "__main__":
    main()
import { Document, Page, pdfjs } from "react-pdf";
import { useEffect, useState } from "react";
import { getDocument } from "pdfjs-dist";
import "react-pdf/dist/Page/AnnotationLayer.css";
import "react-pdf/dist/Page/TextLayer.css";
import { Button, Input, Typography } from "antd";
import { ChevronLeft, ChevronRight } from "lucide-react";

pdfjs.GlobalWorkerOptions.workerSrc = new URL(
  "pdfjs-dist/build/pdf.worker.min.js",
  import.meta.url
).toString();

const PDFReader = () => {
  const [file, setFile] = useState(null);
  const [text, setText] = useState("");
  const [numPages, setNumPages] = useState<number>();
  const [pageNumber, setPageNumber] = useState<number>(1);

  const onFileChange = (event) => {
    setFile(event.target.files[0]);
  };

  const extractText = async (pdfFile) => {
    const reader = new FileReader();
    reader.readAsArrayBuffer(pdfFile);
    reader.onload = async (e) => {
      const typedarray = new Uint8Array(e.target.result);
      const loadingTask = getDocument({ data: typedarray });

      try {
        const pdf = await loadingTask.promise;
        let extractedText = "";

        for (let pageNum = 1; pageNum <= pdf.numPages; pageNum++) {
          const page = await pdf.getPage(pageNum);
          const textContent = await page.getTextContent();
          const strings = textContent.items.map((item) => item.str).join(" ");
          extractedText += strings + " ";
        }

        setText(extractedText);
      } catch (error) {
        console.error("Error extracting text from PDF:", error);
      }
    };
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    if (file) {
      extractText(file);
    }
  };

  function onDocumentLoadSuccess({ numPages }: { numPages: number }): void {
    setNumPages(numPages);
  }

  const nextPage = () => {
    numPages && pageNumber < numPages && setPageNumber(pageNumber + 1);
  };

  const prevPage = () => {
    numPages && pageNumber > 1 && setPageNumber(pageNumber - 1);
  };

  useEffect(() => {
    const down = (e: KeyboardEvent) => {
      if (file) {
        if (e.key === "ArrowRight") {
          setPageNumber((prevPageNumber) => prevPageNumber + 1);
        }
        if (e.key === "ArrowLeft") {
          prevPage();
        }
      }
    };
    document.addEventListener("keydown", down);
    return () => document.removeEventListener("keydown", down);
  }, [file, pageNumber]);

  return (
    <div className='flex gap-4 items-center'>
      <section>
        <form onSubmit={handleSubmit}>
          <input type='file' onChange={onFileChange} accept='application/pdf' />
          <button type='submit'>Extract Text</button>
        </form>
        <div>
          <p>Extracted Text:</p>
          <textarea value={text} readOnly={true} cols={80} rows={20} />
        </div>
      </section>

      {file && (
        <section id='pdfCont' className='h-[900px] w-[695px]'>
          <div className='pdfContainer rounded-lg'>
            <Document
              className='dark:invert'
              style={{ textAlign: "center" }}
              file={file}
              onLoadSuccess={onDocumentLoadSuccess}
            >
              <Page pageNumber={pageNumber} height={900} width={695} />
            </Document>
            <div className='absolute mt-1 flex justify-center rounded-xl bg-white p-1.5 shadow-md dark:bg-gray-800'>
              <button onClick={prevPage} disabled={pageNumber === 1}>
                <ChevronLeft
                  className={pageNumber === 1 ? "text-gray-300" : ""}
                />
              </button>
              <span className='mx-2 flex items-center'>
                {pageNumber} / {numPages}
              </span>
              <button onClick={nextPage} disabled={pageNumber === numPages}>
                <ChevronRight
                  className={pageNumber === numPages ? "text-gray-300" : ""}
                />
              </button>
            </div>
          </div>
        </section>
      )}
    </div>
  );
};

export default PDFReader;
